using StockApp_Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StockApp_Classes.Services
{
    public class SymbolService
    {
        private readonly string filePath;

        public SymbolService(string filePath)
        {
            this.filePath = filePath;
        }


        public List<Symbols> GetAllSymbols()
        {
            using (StreamReader reader = new StreamReader(this.filePath))
            {
                string jsonString = reader.ReadToEnd();
                var symbols = JsonSerializer.Deserialize<CompaniesCollection>(jsonString);

                if (symbols?.Companies == null)
                {
                    throw new Exception("Failed to deserialize JSON file.");
                }

                return symbols.Companies;
            }
                
            
        }
    }
}
