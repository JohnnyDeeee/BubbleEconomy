using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using System.Linq;

namespace BubbleEconomy {
    public enum PopulationType { nonWorking = 0, working = 1 }

    class City {
        private float cityRadius = 60f;

        public Vector2 position { get; private set; }
        public float economy { get; private set; }
        public float nonWorkingPopulation { get; private set; }
        public float workingPopulation { get; private set; }
        public float totalPopulation { get { return nonWorkingPopulation + workingPopulation; } }
        public readonly float economyLimit = 250;
        public readonly float workingLimit = 100;
        public static float neighbourBoundaryRadius = 200f;

        public City(Vector2 position, float startingEconomy, float startingNonWorkingPopulation, float startingWorkingPopulation) {
            this.position = position;
            economy = startingEconomy;
            nonWorkingPopulation = startingNonWorkingPopulation;
            workingPopulation = startingWorkingPopulation;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin();

            spriteBatch.DrawCircle(position, cityRadius, 180, Color.Green, economy * 0.005f);
            spriteBatch.DrawCircle(position, cityRadius + nonWorkingPopulation * 0.05f, 180, Color.Red, nonWorkingPopulation * 0.05f);
            spriteBatch.DrawString(Game1.font, $"Economy: {economy}", position + new Vector2(-cityRadius / 2, 0), Color.White);
            spriteBatch.DrawString(Game1.font, $"Population (working): {workingPopulation}", position + new Vector2(-cityRadius / 2, 20), Color.White);
            spriteBatch.DrawString(Game1.font, $"Population (non-working): {nonWorkingPopulation}", position + new Vector2(-cityRadius / 2, 40), Color.White);

            if (Game1.debug) {
                spriteBatch.DrawCircle(position, neighbourBoundaryRadius, 180, Color.Blue);
            }

            spriteBatch.End();
        }

        public void Update(List<City> cities) {
            // Economy changes every tick
            economy -= nonWorkingPopulation * 0.01f;
            economy += workingPopulation * 0.1f;
            economy = economy > 0 ? economy - 0.1f : 0; // When population is empty, economy should still go down

            for (int i = 0; i < totalPopulation; i++) {
                PopulationType currentPopulation = PopulationType.nonWorking;
                if (i > nonWorkingPopulation || nonWorkingPopulation == 0)
                    currentPopulation = PopulationType.working;

                float chanceToLeave = 0.20f; // % chance to leave
                float chanceToWork = 0.40f; // % chance to work
                if (Util.rand.NextSingle() < chanceToLeave) { // Let population decide to go to another city if it's economy is better
                    if (currentPopulation == PopulationType.nonWorking)
                        nonWorkingPopulation = nonWorkingPopulation > 0 ? nonWorkingPopulation - 1 : 0;
                    else if (currentPopulation == PopulationType.working)
                        workingPopulation = workingPopulation > 0 ? workingPopulation - 1 : 0;

                    List<City> citiesOrderedByEconomy = cities.OrderByDescending(city => city.economy).ToList();
                    citiesOrderedByEconomy.Remove(this); // Cant move to the current city
                    City bestCity = citiesOrderedByEconomy[0];
                    bestCity.addToPopulation(1);
                } else if (Util.rand.NextSingle() < chanceToWork) { // Let nonWorking population decide to go work (if there is room)
                    if (currentPopulation == PopulationType.nonWorking && workingPopulation < workingLimit && nonWorkingPopulation > 0) {
                        workingPopulation += 1;
                        nonWorkingPopulation -= 1;
                    }
                }
                
            }

            // Give excess to neighbours
            if (economy >= economyLimit) {
                List<City> neighbours = new List<City>();
                cities.ForEach(city => {
                    if (NetRumble.CollisionMath.CircleCircleIntersect(position, neighbourBoundaryRadius, city.position, city.cityRadius))
                        neighbours.Add(city);
                });
                float excessEconomy = (economy - economyLimit) / neighbours.Count;
                neighbours.ForEach(x => addToOtherEconomy(excessEconomy, x));
            }
        }

        public void addToEconomy(float amount) {
            economy += amount;
        }

        public void addToOtherEconomy(float amount, City city) {
            economy -= amount;
            city.addToEconomy(amount);
        }

        public void addToPopulation(int amount) {
            nonWorkingPopulation += amount;
        }
    }
}
