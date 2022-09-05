using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Newtonsoft.Json;

namespace Platformer
{
    public class Level
    {
        public struct TileData
        {
            public TileType Type = TileType.None;
            public uint Count = 1;

            public TileData()
            {
                Type = TileType.None;
                Count = 1;
            }

            public TileData(TileType type)
            {
                Type = type;
            }

            public TileData(TileType type, uint count)
            {
                Type = type;
                Count = count;
            }
        }

        public struct JSONTileData
        {
            public string type;
            public uint count = 1;

            public JSONTileData(string type, uint count)
            {
                this.type = type;
                this.count = count;
            }
        }

        protected Vector2f beginPosition = new Vector2f(0, 0);
        protected int tileScaleFactor = 4;
        protected List<List<TileData>> tilesData = new();
        protected List<Tile> tiles = new();
        protected List<Tile> backgroundTiles = new();

        public Level()
        {
            EventsHandler.Add_On_Resize_Event(Window_Resize_Event);
        }

        public Level(int tileScaleFactor)
        {
            this.tileScaleFactor = tileScaleFactor;

            EventsHandler.Add_On_Resize_Event(Window_Resize_Event);
        }

        public void Generate_Level()
        {
            tiles = new();

            beginPosition = Get_Begin_Position();

            float top = beginPosition.Y;
            float left = beginPosition.X;

            Vector2f defaultTileSize = TileHelper.Get_Texture_Size(TileType.None) * tileScaleFactor;

            for (int i = 0, iLength = tilesData.Count; i < iLength; i++)
            {
                for (int j = 0, jLength = tilesData[i].Count; j < jLength; j++)
                {
                    TileData tileData = tilesData[i][j];

                    Vector2f additionalOffset = new(0, 0);

                    if (tileData.Type == TileType.TreeLeaf)
                    {
                        additionalOffset = new(8, -8);
                    }

                    additionalOffset *= tileScaleFactor;

                    if (tileData.Type == TileType.None)
                    {
                        left += tileData.Count * TileHelper.Get_Texture_Size(tileData.Type).X * tileScaleFactor;
                        continue;
                    }

                    for (int z = 0; z < tileData.Count; z++)
                    {
                        Vector2f position = new Vector2f(left, top);
                        Vector2f size = TileHelper.Get_Texture_Size(tileData.Type) * tileScaleFactor;

                        Vector2f positionOffset = defaultTileSize - size;

                        tiles.Add(new Tile(position + positionOffset + additionalOffset, size, tileData.Type));

                        left += defaultTileSize.X;
                    }
                }

                top += defaultTileSize.Y;
                left = beginPosition.X;
            }

            Create_Background();
        }

        private void Create_Background()
        {
            Vector2f tileSize = TileHelper.Get_Texture_Size(TileType.BackgroundHills) * tileScaleFactor;

            int col = (int)Math.Round((tilesData[0].Count * TileHelper.Get_Texture_Size(TileType.None).X * tileScaleFactor) / tileSize.X) + 1;

            Vector2f position = beginPosition;

            for (int i = 0; i < col; i++)
            {
                backgroundTiles.Add(new Tile(position, tileSize, TileType.BackgroundHills));
                position += new Vector2f(tileSize.X, 0);
            }
        }

        public void Draw_Background(RenderWindow window)
        {
            Vector2f skySize = new Vector2f(tilesData[0].Count * TileHelper.Get_Texture_Size(TileType.None).X * tileScaleFactor, WindowResizeObserver.Size.Y);

            RectangleShape sky = new(skySize)
            {
                Position = new(beginPosition.X, beginPosition.Y - skySize.Y),
                FillColor = new Color(19, 12, 55)
            };

            window.Draw(sky);

            backgroundTiles.ForEach(delegate (Tile tile)
            {
                tile.Draw(window);
            });
        }

        private void Window_Resize_Event(object? sender, SizeEventArgs e)
        {
            Update_Level();

            beginPosition = Get_Begin_Position();
        }

        virtual public Vector2f Get_Begin_Position()
        {
            return new(0, WindowResizeObserver.Size.Y - tilesData.Count * 8 * tileScaleFactor);
        }

        private void Update_Level()
        {
            Vector2f beginPositionDiff = Get_Begin_Position() - beginPosition;

            tiles.ForEach(delegate (Tile tile)
            {
                tile.Position += new Vector2f(0, beginPositionDiff.Y);
            });

            backgroundTiles.ForEach(delegate (Tile tile)
            {
                tile.Position += new Vector2f(0, beginPositionDiff.Y);
            });
        }

        public void Draw(RenderWindow window)
        {
            tiles.ForEach(delegate (Tile tile)
            {
                tile.Draw(window);
            });
        }

        public void Load_Data_From_JSON(string fileName)
        {
            string currentDirectoryPath = Directory.GetCurrentDirectory();

            string absPath = Path.Combine(currentDirectoryPath, "..\\..\\..\\", fileName);

            using StreamReader r = new(absPath);
            string json = r.ReadToEnd();

            List<List<JSONTileData>>? items = JsonConvert.DeserializeObject<List<List<JSONTileData>>>(json);

            tilesData = (items ?? (new())).ConvertAll(new Converter<List<JSONTileData>, List<TileData>>(Convert_Json_Row));
        }

        private List<TileData> Convert_Json_Row(List<JSONTileData> row)
        {
            return row.ConvertAll(new Converter<JSONTileData, TileData>(Convert_Json));
        }

        private TileData Convert_Json(JSONTileData data)
        {
            return new TileData((TileType)data.type, data.count);
        }

        public List<List<TileData>> TilesData
        {
            set => tilesData = value;
            get => tilesData;
        }

        public Vector2f BeginPosition
        {
            set => beginPosition = value;
        }
    }
}
