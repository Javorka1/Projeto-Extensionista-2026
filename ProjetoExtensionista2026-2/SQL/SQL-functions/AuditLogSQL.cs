using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
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
                        CREATE TABLE IF NOT EXISTS logs (
                            id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                            username VARCHAR(255) NOT NULL,
                            action VARCHAR(255) NULL,
                            created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                            INDEX idx_logs_created_at (created_at)
                        ) ENGINE=InnoDB;";
                    command.ExecuteNonQuery();

                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS low_stock_notifications (
                            id BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
                            product_id INT NOT NULL,
                            product_name VARCHAR(200) NOT NULL,
                            stock_quantity DECIMAL(12,3) NOT NULL,
                            min_stock DECIMAL(12,3) NOT NULL,
                            created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                            displayed_at TIMESTAMP NULL,
                            INDEX idx_low_stock_product (product_id),
                            INDEX idx_low_stock_displayed (displayed_at)
                        ) ENGINE=InnoDB;";
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
                    using (var adapter = new MySqlDataAdapter((MySqlCommand)command))
                    {
                        adapter.Fill(table);
                    }
                }
            }
            return table;
        }
    }
}
