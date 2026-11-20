using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Windows.Forms;

namespace ProjetoExtensionista2026_2.SQL.SQL_functions
{
    internal static class AuditLogSQL
    {
        private sealed class LowStockNotification
        {
            public long Id { get; set; }
            public string ProductName { get; set; }
            public decimal StockQuantity { get; set; }
            public decimal MinimumStock { get; set; }
        }

        private static readonly Queue<LowStockNotification> FilaNotificacoes =
            new Queue<LowStockNotification>();
        private static Timer _timerNotificacoes;
        private static NotifyIcon _iconeNotificacoes;

        public static void NotifyLowStock(NotifyIcon notificationIcon)
        {
            if (notificationIcon == null)
                return;

            try
            {
                var items = new List<LowStockNotification>();
                using (var connection = new Connection())
                {
                    connection.IniciarConexao();
                    using (var command = connection.Conexao.CreateCommand())
                    {
                        command.CommandText = @"
                            SELECT id, product_name, stock_quantity, min_stock
                            FROM low_stock_notifications
                            WHERE displayed_at IS NULL
                            ORDER BY created_at ASC
                            LIMIT 10;";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new LowStockNotification
                                {
                                    Id = Convert.ToInt64(reader["id"]),
                                    ProductName = Convert.ToString(reader["product_name"]),
                                    StockQuantity = Convert.ToDecimal(reader["stock_quantity"]),
                                    MinimumStock = Convert.ToDecimal(reader["min_stock"])
                                });
                            }
                        }
                    }
                }

                if (items.Count == 0)
                    return;

                FilaNotificacoes.Clear();
                foreach (LowStockNotification item in items)
                    FilaNotificacoes.Enqueue(item);

                _iconeNotificacoes = notificationIcon;
                if (_timerNotificacoes == null)
                {
                    _timerNotificacoes = new Timer
                    {
                        Interval = 3500
                    };
                    _timerNotificacoes.Tick += (s, e) => MostrarProximaNotificacao();
                }

                _timerNotificacoes.Stop();
                MostrarProximaNotificacao();
                if (FilaNotificacoes.Count > 0)
                    _timerNotificacoes.Start();
            }
            catch
            {
            }
        }

        private static void MostrarProximaNotificacao()
        {
            if (_iconeNotificacoes == null || FilaNotificacoes.Count == 0)
            {
                _timerNotificacoes?.Stop();
                return;
            }

            LowStockNotification item = FilaNotificacoes.Dequeue();
            _iconeNotificacoes.BalloonTipTitle = "Alerta de estoque";
            _iconeNotificacoes.BalloonTipText =
                $"O item {item.ProductName} está com estoque baixo " +
                $"({item.StockQuantity:N0}/{item.MinimumStock:N0}).";
            _iconeNotificacoes.ShowBalloonTip(3000);
            MarcarComoExibida(item.Id);

            if (FilaNotificacoes.Count == 0)
                _timerNotificacoes.Stop();
        }

        private static void MarcarComoExibida(long notificationId)
        {
            using (var connection = new Connection())
            {
                connection.IniciarConexao();
                using (var command = connection.Conexao.CreateCommand())
                {
                    command.CommandText = @"
                        UPDATE low_stock_notifications
                        SET displayed_at = NOW()
                        WHERE id = @id AND displayed_at IS NULL;";
                    command.Parameters.AddWithValue("id", notificationId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void StopLowStockNotifications()
        {
            _timerNotificacoes?.Stop();
            FilaNotificacoes.Clear();
            _iconeNotificacoes = null;
        }

        public static void EnsureSchema()
        {
            using (var connection = new Connection())
            {
                connection.IniciarConexao();
                using (var command = connection.Conexao.CreateCommand())
                {
                    command.CommandText = @"
                        ALTER TABLE ""Users"" ADD COLUMN IF NOT EXISTS active BOOLEAN NOT NULL DEFAULT TRUE;
                        ALTER TABLE ""Users"" ADD COLUMN IF NOT EXISTS created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW();
                        UPDATE ""Users"" SET created_at = NOW() WHERE created_at IS NULL;
                        CREATE TABLE IF NOT EXISTS logs (
                            id BIGSERIAL PRIMARY KEY,
                            created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
                            action VARCHAR(120) NOT NULL,
                            username VARCHAR(80) NOT NULL
                        );
                        CREATE TABLE IF NOT EXISTS low_stock_notifications (
                            id BIGSERIAL PRIMARY KEY,
                            product_id INTEGER NOT NULL,
                            product_name VARCHAR(200) NOT NULL,
                            stock_quantity NUMERIC(12,3) NOT NULL,
                            min_stock NUMERIC(12,3) NOT NULL,
                            created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
                            displayed_at TIMESTAMP WITH TIME ZONE NULL
                        );
                        CREATE OR REPLACE FUNCTION fn_product_stock_alert()
                        RETURNS TRIGGER AS $low_stock$
                        BEGIN
                            IF TG_OP = 'INSERT' THEN
                                IF NEW.is_active = TRUE AND NEW.stock_quantity <= NEW.min_stock THEN
                                    INSERT INTO low_stock_notifications
                                        (product_id, product_name, stock_quantity, min_stock)
                                    VALUES
                                        (NEW.id_product, NEW.name, NEW.stock_quantity, NEW.min_stock);
                                END IF;
                            ELSIF NEW.is_active = TRUE
                                  AND NEW.stock_quantity <= NEW.min_stock
                                  AND (OLD.is_active = FALSE
                                       OR OLD.stock_quantity > OLD.min_stock
                                       OR OLD.min_stock <> NEW.min_stock) THEN
                                INSERT INTO low_stock_notifications
                                    (product_id, product_name, stock_quantity, min_stock)
                                VALUES
                                    (NEW.id_product, NEW.name, NEW.stock_quantity, NEW.min_stock);
                            END IF;

                            RETURN NEW;
                        END;
                        $low_stock$ LANGUAGE plpgsql;
                        DROP TRIGGER IF EXISTS trg_product_stock_alert ON public.product;
                        CREATE TRIGGER trg_product_stock_alert
                        AFTER INSERT OR UPDATE OF name, stock_quantity, min_stock, is_active ON public.product
                        FOR EACH ROW EXECUTE FUNCTION fn_product_stock_alert();";
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void Register(string action, string username)
        {
            if (string.IsNullOrWhiteSpace(action))
                return;

            try
            {
                using (var connection = new Connection())
                {
                    connection.IniciarConexao();
                    using (var command = connection.Conexao.CreateCommand())
                    {
                        command.CommandText = @"
                            INSERT INTO logs (created_at, action, username)
                            VALUES (NOW(), @action, @username);";
                        command.Parameters.AddWithValue("action", action);
                        command.Parameters.AddWithValue("username", string.IsNullOrWhiteSpace(username) ? "Sistema" : username);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {

            }
        }

        public static DataTable GetLogs()
        {
            var table = new DataTable();
            using (var connection = new Connection())
            {
                connection.IniciarConexao();
                using (var command = connection.Conexao.CreateCommand())
                {
                    command.CommandText = @"
                         SELECT created_at AS occurred_at, action, username
                        FROM logs
                         ORDER BY created_at DESC
                        LIMIT 500;";
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        adapter.Fill(table);
                    }
                }
            }
            return table;
        }
    }
}
