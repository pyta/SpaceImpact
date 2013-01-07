namespace GRProjekt.Game.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Material
    {
        private MaterialType materialType { get; set; }
        private float currentValue { get; set; }
        private float maxValue { get; set; }

        private float currentPriceForSell { get; set; }
        private float maxPriceForSell { get; set; }
        private float defaultPriceForSell { get; set; }

        private float currentPriceForBuy { get; set; }
        private float maxPriceForBuy { get; set; }

        private float productionFactor { get; set; }

        public Material(MaterialType mT, float cV, float mV, float cP4S, float mP4S, float dP4S, float cP4B, float mP4B, float production)
        {
            this.materialType = mT;
            this.currentValue = cV;
            this.maxValue = mV;
            this.currentPriceForSell = cP4S;
            this.maxPriceForSell = mP4S;
            this.defaultPriceForSell = mP4S;
            this.currentPriceForBuy = cP4B;
            this.maxPriceForBuy = mP4B;
            this.productionFactor = production;
        }

        public void UpdateMaterial(float currentPopulation, float prosperity)
        {
            currentValue += (currentPopulation / 10) * (productionFactor/100) * (prosperity/100);
            if (currentValue > maxValue)
            {
                currentValue = maxValue;
            }
            else if (currentValue < 0)
            {
                currentValue = 0;
            }

            float stateOfMaterial = currentValue/maxValue;
            if (stateOfMaterial > 0.8f)
            {
                stateOfMaterial = 0.8f;
            }
            
            currentPriceForSell = defaultPriceForSell - (stateOfMaterial * defaultPriceForSell);
            if (currentPriceForSell > maxPriceForSell || currentPriceForSell < maxPriceForSell)
            {
                currentPriceForSell = maxPriceForSell;
            }

            currentPriceForBuy = (defaultPriceForSell/2) + (stateOfMaterial * defaultPriceForSell);
            if (currentPriceForBuy > maxPriceForBuy || currentPriceForBuy < maxPriceForBuy)
            {
                currentPriceForBuy = maxPriceForBuy;
            }
        }

        public float radnomDelivery()
        {
            Random rand = new Random();
            float percent = rand.Next(5, 15) / 100;
            if (productionFactor >0)
            {
                currentValue -= maxValue * percent;
                return maxValue * percent * currentPriceForSell;
            }
            else if (productionFactor < 0)
            {
                currentValue += maxValue * percent;
                return maxValue * percent * currentPriceForBuy * -1;
            }
            return 0;
        }
    }
}
