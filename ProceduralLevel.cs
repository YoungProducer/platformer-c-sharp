using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SFML.System;

namespace Platformer
{
    public class ProceduralLevel : Level
    {
        public enum FindDirection
        {
            Top,
            Bottom
        }

        Random random = new();
        RangeConverter converter;
        Vector2i levelSize = new(500, 40);
        int interpolationStep = 7;
        int minimumTreeGap = 10;
        float treeProbability = 0.8f;
        float surfaceDetailsProbablility = 0.8f;
        // grass to flowers ratio
        float surfaceDetailsRatio = 0.8f;

        public ProceduralLevel(int tileScaleFactor) : base(tileScaleFactor)
        {
            this.tileScaleFactor = tileScaleFactor;
            SimplexNoise.Noise.Seed = random.Next(0, 2147483647);
            //SimplexNoise.Noise.Seed = 10;
            converter = new(0, 255, 1, levelSize.Y);
        }

        public void Generate_Tile_Data()
        {
            Init_Tile_Data();

            Run_Terrain_Procedural_Generation();

            Beautify();

            Run_Tree_Generation();

            Run_Flora_Generation();
        }

        private void Run_Terrain_Procedural_Generation()
        {
            int i = 0;
            int x = 0;

            float[] noise = Get_Noise();

            float lValue = noise[0];

            Vector2f lastPoint = new Vector2f(i, (int)converter.Map(lValue) - 1);

            while (i < levelSize.X)
            {
                float nValue = noise[x + 1];
                Vector2f nextPoint = new Vector2f(i + interpolationStep, (int)converter.Map(nValue) - 1);

                List<Vector2f> pointsRange = LinearInterpolation.InterpolateRange(lastPoint, nextPoint);

                tilesData[(int)lastPoint.Y][(int)lastPoint.X] = new TileData(TileType.GrassTop, 1);

                for (int j = 0; j < pointsRange.Count; j++)
                {
                    Vector2i point = new((int)pointsRange[j].X, (int)pointsRange[j].Y);

                    tilesData[point.Y][point.X] = new TileData(TileType.GrassTop, 1);
                }

                i += interpolationStep;
                x++;
                lastPoint = nextPoint;
            }
        }

        private void Run_Tree_Generation()
        {
            RangeConverter localConverter = new(0, 255, 0, 10);

            float[] noise = SimplexNoise.Noise.Calc1D(1000, 0.90f);

            for (int col = 1, width = levelSize.X - 1; col < width; col++)
            {
                for (int row = 1, height = levelSize.Y - 3; row < height; row++)
                {
                    TileData currentTile = tilesData[row][col];

                    if (currentTile.Type != TileType.GrassTop) continue;
                    if (tilesData[row][col - 1].Type == TileType.Ladder || tilesData[row][col + 1].Type == TileType.Ladder) continue;

                    float noiseValue = noise[col];
                    int mappedValue = (int)localConverter.Map(noiseValue);

                    if (mappedValue < 10 - 10 * treeProbability) continue;

                    tilesData[row - 1][col] = new(TileType.TreeRoot);
                    tilesData[row - 2][col] = new(TileType.Tree);
                    tilesData[row - 3][col] = new(TileType.TreeLeaf);
                }

                if (col + minimumTreeGap > levelSize.X - 1) break;

                col += minimumTreeGap;
            }
        }

        private void Run_Flora_Generation()
        {
            RangeConverter localConverter = new(0, 255, 0, 10);

            float[] noise = SimplexNoise.Noise.Calc1D(1000, 0.90f);

            float minimum = 10 - 10 * surfaceDetailsProbablility;
            float flowersMinimum = 10 - (10 - minimum * surfaceDetailsRatio);

            for (int col = 1, width = levelSize.X; col < width; col++)
            {
                for (int row = 1, height = levelSize.Y - 1; row < height; row++)
                {
                    TileData currentTile = tilesData[row][col];
                    TileData topTile = tilesData[row - 1][col];

                    if (!(currentTile.Type == TileType.GrassTop || currentTile.Type == TileType.GrassTopLeftOutterCorner || currentTile.Type == TileType.GrassTopRightOutterCorner) || currentTile.Type == TileType.Ladder
                        ) continue;

                    if (topTile.Type == TileType.TreeRoot || topTile.Type == TileType.Ladder) continue;

                    float noiseValue = noise[col];
                    double mappedValue = localConverter.Map(noiseValue);

                    if (mappedValue < minimum) continue;

                    tilesData[row - 1][col] = new TileData(mappedValue - flowersMinimum > flowersMinimum ? TileType.GrassTopDetails : TileType.Flowers);
                }
            }
        }

        private void Beautify()
        {
            List<Vector2i> topGrasses = new();

            int firstStartGrassY = Find_First_Grass_From_Start();
            tilesData[firstStartGrassY][0] = new TileData(TileType.GrassTop);
            topGrasses.Add(new Vector2i(0, firstStartGrassY));

            for (int row = 1, height = levelSize.Y - 1; row < height; row++)
            {
                for (int col = 1, width = levelSize.X; col < width; col++)
                {
                    bool notGrassTopAnymore = false;

                    TileData currentTile = tilesData[row][col];
                    TileData previousTile = tilesData[row][col - 1];
                    TileData previousTopTile = tilesData[row - 1][col - 1];
                    TileData? nextTile = col < levelSize.X - 1 ? tilesData[row][col + 1] : null;
                    TileData? nextTopTile = col < levelSize.X - 1 ? tilesData[row - 1][col + 1] : null;

                    if (nextTile != null && currentTile.Type == TileType.None)
                    {
                        if (Check_And_Fix_Gutter(previousTile, (TileData)nextTile, col, row))
                        {
                            topGrasses.Add(new(col, row));
                            continue;
                        }
                    }

                    if (currentTile.Type == TileType.None ||
                        currentTile.Type == TileType.GrassBottomLeftInnerCorner ||
                        currentTile.Type == TileType.GrassBottomRightInnerCorner ||
                        currentTile.Type == TileType.GrassLeft ||
                        currentTile.Type == TileType.GrassRight ||
                        currentTile.Type == TileType.Ladder
                        ) continue;

                    if (nextTile != null)
                    {
                        if (Check_And_Fix_Picks(previousTile, (TileData)nextTile, col, row))
                        {
                            topGrasses.Add(new(col, row + 1));
                            continue;
                        }
                    }

                    if (
                        previousTile.Type == TileType.None &&
                        previousTopTile.Type != TileType.GrassTopRightOutterCorner
                        )
                    {
                        tilesData[row][col] = new TileData(TileType.GrassTopLeftOutterCorner);

                        int closestGrassY = Find_Closest_Grass(FindDirection.Bottom, col - 1, row + 1, levelSize.Y);

                        Vertical_Fill(col, row + 1, closestGrassY, TileType.GrassLeft);

                        if (closestGrassY != -1)
                        {
                            tilesData[closestGrassY][col] = new TileData(TileType.GrassBottomRightInnerCorner);

                            notGrassTopAnymore = true;

                            topGrasses.Add(new Vector2i(col, closestGrassY));
                        }
                    }

                    if (nextTile != null && nextTopTile != null)
                    {
                        if (
                            ((TileData)nextTile).Type == TileType.None &&
                            ((TileData)nextTopTile).Type != TileType.GrassTopLeftOutterCorner
                            )
                        {
                            tilesData[row][col] = new TileData(TileType.GrassTopRightOutterCorner);

                            int closestGrassY = Find_Closest_Grass(FindDirection.Bottom, col + 1, row + 1, levelSize.Y);

                            Vertical_Fill(col, row + 1, closestGrassY, TileType.GrassRight);

                            if (closestGrassY != -1)
                            {
                                tilesData[closestGrassY][col] = new TileData(TileType.GrassBottomLeftInnerCorner);

                                notGrassTopAnymore = true;

                                topGrasses.Add(new Vector2i(col, closestGrassY));
                            }
                        }
                    }

                    if (currentTile.Type == TileType.GrassTop && !notGrassTopAnymore)
                    {
                        topGrasses.Add(new Vector2i(col, row));
                    }
                }
            }

            // Clean Artifacts
            for (int row = 1, height = levelSize.Y - 1; row < height; row++)
            {
                for (int col = 1, width = levelSize.X - 1; col < width; col++)
                {
                    TileData currentTile = tilesData[row][col];
                    TileData bottomTile = tilesData[row + 1][col];

                    if ((bottomTile.Type == TileType.GrassBottomLeftInnerCorner || bottomTile.Type == TileType.GrassBottomRightInnerCorner) &&
                        currentTile.Type == TileType.GrassTop)
                    {
                        tilesData[row + 1][col] = new TileData(TileType.Dirt);
                    }
                }
            }

            for (int row = 0, height = levelSize.Y; row < height; row++)
            {
                for (int col = 0, width = levelSize.X; col < width; col++)
                {
                    TileData currentTile = tilesData[row][col];

                    if (currentTile.Type == TileType.GrassTop)
                    {
                        Vertical_Fill(col, row + 1, levelSize.Y - 1, TileType.Dirt);
                    }

                    if (currentTile.Type == TileType.GrassTopLeftOutterCorner ||
                        currentTile.Type == TileType.GrassTopRightOutterCorner
                        )
                    {
                        for (int i = row; i < height; i++)
                        {
                            if (tilesData[i][col].Type == TileType.GrassBottomLeftInnerCorner ||
                                tilesData[i][col].Type == TileType.GrassBottomRightInnerCorner)
                            {
                                Vertical_Fill(col, i + 1, levelSize.Y - 1, TileType.Dirt);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private bool Check_And_Fix_Gutter(TileData previousTile, TileData nextTile, int col, int row)
        {
            if (previousTile.Type == TileType.GrassTopRightOutterCorner &&
                nextTile.Type == TileType.GrassTop
                )
            {
                tilesData[row][col] = new TileData(TileType.GrassTop);
                tilesData[row][col - 1] = new TileData(TileType.GrassTop);
                return true;
            }

            return false;
        }


        private bool Check_And_Fix_Picks(TileData previousTile, TileData nextTile, int col, int row)
        {
            if (previousTile.Type == TileType.None && nextTile.Type == TileType.None)
            {
                tilesData[row][col] = new TileData(TileType.None);
                tilesData[row + 1][col] = new TileData(TileType.GrassTop);

                return true;
            }

            return false;
        }

        private int Find_Closest_Grass(FindDirection direction, int col, int rowStart, int rowEnd)
        {
            if (direction == FindDirection.Top)
            {
                for (int i = rowEnd - 1; i > rowStart; i--)
                {
                    TileData tileData = tilesData[i][col];

                    if (tileData.Type == TileType.GrassTop ||
                        tileData.Type == TileType.GrassBottomLeftInnerCorner ||
                        tileData.Type == TileType.GrassBottomRightInnerCorner
                        )
                    {
                        return i;
                    }
                }
            }

            if (direction == FindDirection.Bottom)
            {
                for (int i = rowStart; i < rowEnd; i++)
                {
                    TileData tileData = tilesData[i][col];

                    if (tileData.Type == TileType.GrassTop ||
                        tileData.Type == TileType.GrassTopLeftOutterCorner ||
                        tileData.Type == TileType.GrassTopRightOutterCorner
                        )
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private int Find_First_Grass_From_Start()
        {
            for (int row = 1, height = levelSize.Y - 1; row < height; row++)
            {
                if (tilesData[row][0].Type == TileType.GrassTop) return row;
            }

            return 0;
        }

        private void Vertical_Fill(int col, int rowStart, int rowEnd, TileType type)
        {
            for (int i = rowStart; i < rowEnd; i++)
            {
                tilesData[i][col] = new TileData(type);
            }
        }

        private void Init_Tile_Data()
        {
            int width = (int)(levelSize.X / interpolationStep) * interpolationStep;

            levelSize = new Vector2i(width, levelSize.Y);

            for (int i = 0; i < levelSize.Y; i++)
            {
                tilesData.Add(new());

                for (int j = 0; j < width; j++)
                {
                    tilesData[i].Add(new TileData());
                }
            }
        }

        private static float[] Get_Noise()
        {
            float[] noiseHigh = SimplexNoise.Noise.Calc1D(1000, 0.90f);
            float[] noiseSmooth = SimplexNoise.Noise.Calc1D(1000, 0.30f);
            //float[] noiseSmooth = SimplexNoise.Noise.Calc1D(1000, 0.80f);
            float[] noise = new float[1000];

            for (int i = 0; i < noiseHigh.Length; i++)
            {
                noise[i] = (noiseHigh[i] + noiseSmooth[i]) / 2;
            }

            return noise;
        }

        override public Vector2f Get_Begin_Position()
        {
            return new(0, WindowResizeObserver.Size.Y - (tilesData.Count - 1) * 8 * tileScaleFactor);
        }

        public Vector2i Get_Level_Size_Px()
        {
            return levelSize * 8 * tileScaleFactor;
        }
    }
}
