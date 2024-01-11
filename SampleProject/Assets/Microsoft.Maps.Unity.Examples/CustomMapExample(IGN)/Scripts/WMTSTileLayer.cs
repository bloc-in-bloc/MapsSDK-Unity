using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;

public class WMTSTileLayer : TextureTileLayer {
    public string url;
    
    public override Task<TextureTile?> GetTexture(TileId tileId, CancellationToken cancellationToken = default) {
        var tilePosition = tileId.ToTilePosition();
        var requestUrl = $"{url}&TILEMATRIX={tilePosition.LevelOfDetail}&TILECOL={tilePosition.X}&TILEROW={tilePosition.Y}";
        return Task.FromResult<TextureTile?>(TextureTile.FromUrl(new Uri(requestUrl)));
    }
}
