using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;

public class COGTileLayer : TextureTileLayer {
    public string url;
    
    public override Task<TextureTile?> GetTexture(TileId tileId, CancellationToken cancellationToken = default) {
        var tilePosition = tileId.ToTilePosition();
        var requestUrl = $"http://tiles.rdnt.io/tiles/{tilePosition.LevelOfDetail}/{tilePosition.X}/{tilePosition.Y}?url={url}&nodata=255";
        // Debug.Log (requestUrl);
        return Task.FromResult<TextureTile?>(TextureTile.FromUrl(new Uri(requestUrl)));
    }
}
