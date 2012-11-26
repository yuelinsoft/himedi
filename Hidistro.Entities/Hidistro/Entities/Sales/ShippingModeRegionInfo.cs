namespace Hidistro.Entities.Sales
{
    using System;

    [Serializable]
    public class ShippingModeRegionInfo
    {
       int? formulaId;
       int modeId;
       int regionId;

        public ShippingModeRegionInfo(int modeId, int regionId, int? formulaId)
        {
            this.modeId = modeId;
            this.regionId = regionId;
            this.formulaId = formulaId;
        }

        public int? FormulaId
        {
            get
            {
                return this.formulaId;
            }
        }

        public int ModeId
        {
            get
            {
                return this.modeId;
            }
        }

        public int RegionId
        {
            get
            {
                return this.regionId;
            }
        }
    }
}

