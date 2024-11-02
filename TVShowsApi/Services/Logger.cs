namespace TVShowsApi.Services
{
    public class Logger
    {
        private readonly string _logFilePath;

        public Logger()
        {
            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _logFilePath = Path.Combine(directory, "logs.txt");
        }

        public void LogError(string message)
        {
            try
            {
                using (var writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log error: {ex.Message}");
            }
        }
    }
}
