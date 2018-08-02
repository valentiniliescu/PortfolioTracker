using System;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PortfolioTracker.Model;

namespace PortfolioTracker.PAS
{
    public sealed class FilePortfolioStore : IPortfolioStore
    {
        private const string StorageFileName = "portfolio.json";

        [NotNull] private static readonly string DefaultStorageFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), StorageFileName);

        [NotNull] private readonly string _filePath;

        public FilePortfolioStore([NotNull] string filePath)
        {
            _filePath = filePath;
        }

        public FilePortfolioStore() : this(DefaultStorageFilePath)
        {
        }

        public Portfolio Load()
        {
            try
            {
                Portfolio portfolio;

                if (File.Exists(_filePath))
                {
                    string json = File.ReadAllText(_filePath);
                    portfolio = JsonConvert.DeserializeObject<Portfolio>(json, new PortfolioJsonConverter());
                    if (portfolio == null)
                    {
                        throw new NullReferenceException();
                    }
                }
                else
                {
                    portfolio = new Portfolio();
                }

                return portfolio;
            }
            catch (Exception exception)
            {
                throw new PortfolioStoreLoadException("Error loading portfolio", exception);
            }
        }

        public void Save(Portfolio portfolio)
        {
            try
            {
                if (portfolio != null)
                {
                    string json = JsonConvert.SerializeObject(portfolio, Formatting.Indented, new PortfolioJsonConverter());
                    File.WriteAllText(_filePath, json);
                }
                else
                {
                    if (File.Exists(_filePath))
                    {
                        File.Delete(_filePath);
                    }
                }
            }
            catch (Exception exception)
            {
                throw new PortfolioStoreSaveException("Error saving portfolio", exception);
            }
        }
    }
}