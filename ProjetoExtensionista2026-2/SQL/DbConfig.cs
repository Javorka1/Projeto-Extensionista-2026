using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ProjetoExtensionista2026_2.SQL
{
    [DataContract]
    public class DbConfig
    {
        [DataMember]
        public string Server { get; set; } = "127.0.0.1";

        [DataMember]
        public uint Port { get; set; } = 3306;

        [DataMember]
        public string User { get; set; } = "root";

        [DataMember]
        public string Password { get; set; } = string.Empty;

        [DataMember]
        public string Database { get; set; } = "sistema";

        public static string DefaultDirectory =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NexStore2026");

        public static string DefaultConfigPath =>
            Path.Combine(DefaultDirectory, "dbconfig.json");

        private static string PointerFilePath =>
            Path.Combine(DefaultDirectory, "dbconfig.path");

        public static string GetActiveConfigPath()
        {
            try
            {
                if (File.Exists(PointerFilePath))
                {
                    string customPath = File.ReadAllText(PointerFilePath, Encoding.UTF8).Trim();
                    if (!string.IsNullOrWhiteSpace(customPath))
                        return customPath;
                }
            }
            catch
            {
                // Fallback para o caminho padrão se não conseguir ler o ponteiro.
            }

            return DefaultConfigPath;
        }

        public static void SetActiveConfigPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            string normalizedTarget = Path.GetFullPath(path.Trim());
            string normalizedDefault = Path.GetFullPath(DefaultConfigPath);

            if (!Directory.Exists(DefaultDirectory))
                Directory.CreateDirectory(DefaultDirectory);

            if (string.Equals(normalizedTarget, normalizedDefault, StringComparison.OrdinalIgnoreCase))
            {
                if (File.Exists(PointerFilePath))
                    File.Delete(PointerFilePath);
            }
            else
            {
                File.WriteAllText(PointerFilePath, normalizedTarget, Encoding.UTF8);
            }
        }

        public static DbConfig LoadFromJson(string path = null)
        {
            string targetPath = string.IsNullOrWhiteSpace(path) ? GetActiveConfigPath() : path;
            if (!File.Exists(targetPath))
                return null;

            try
            {
                byte[] bytes = File.ReadAllBytes(targetPath);
                using (var stream = new MemoryStream(bytes))
                {
                    var serializer = new DataContractJsonSerializer(typeof(DbConfig));
                    var config = serializer.ReadObject(stream) as DbConfig;
                    if (config != null && string.IsNullOrWhiteSpace(config.Database))
                        config.Database = "sistema";
                    if (config != null && config.Port == 0)
                        config.Port = 3306;
                    return config;
                }
            }
            catch
            {
                return null;
            }
        }

        public static void SaveToJson(DbConfig config, string path = null)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            string targetPath = string.IsNullOrWhiteSpace(path) ? GetActiveConfigPath() : path.Trim();
            string directory = Path.GetDirectoryName(targetPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (string.IsNullOrWhiteSpace(config.Database))
                config.Database = "sistema";

            using (var stream = new MemoryStream())
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, ownsStream: false, indent: true, indentChars: "  "))
                {
                    var serializer = new DataContractJsonSerializer(typeof(DbConfig));
                    serializer.WriteObject(writer, config);
                    writer.Flush();
                }

                File.WriteAllBytes(targetPath, stream.ToArray());
            }

            SetActiveConfigPath(targetPath);
            Connection.ResetCache();
        }
    }
}
