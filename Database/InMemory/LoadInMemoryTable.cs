using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Enums;

namespace Database.InMemory {
    public class LoadInMemoryTable {
        private Dictionary<int, Load> loadTable = new Dictionary<int, Load>();

        public void Insert(Load newLoad, FileType fileType) {
            foreach (Load load in loadTable.Values) {
                if (newLoad.Timestamp == load.Timestamp) {
                    Update(newLoad, load, fileType);
                }
            }

            loadTable.Add(newLoad.Id, newLoad);
        }

        public List<Load> ReadForDeviationCalculation() {
            List<Load> loads = new List<Load>();
            
            foreach (Load load in loadTable.Values) {
                if (load.MeasuredValue != -1 && load.ForecastValue != -1) {
                    loads.Add(new Load(load.Id, load.ForecastValue, load.MeasuredValue));
                }
            }

            return loads;
        }

        public void UpdateDeviations(List<Load> loads, DeviationType deviationType) {
            foreach (Load load in loads) {
                if (loadTable.Values.Count > 0) {
                    if (deviationType == DeviationType.SquDeviation) {
                        loadTable[load.Id].SquaredDeviation = load.SquaredDeviation;
                    } else {
                        loadTable[load.Id].AbsolutePercentageDeviation = load.AbsolutePercentageDeviation;
                    }
                }
            }
        }

        private void Update(Load newLoad, Load load, FileType fileType) {
            if (fileType == FileType.Ostv) {
                load.ForecastValue = newLoad.ForecastValue;
                load.ForecastFileID = newLoad.ForecastFileID;
            } else {
                load.MeasuredValue = newLoad.MeasuredValue;
                load.MeasuredFileId = newLoad.MeasuredFileId;
            }
        }
    }
}
