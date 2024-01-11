using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using UnityEngine;
using UnityEngine.Networking;

[ExecuteAlways]
public class CustomWMSElevation : ElevationTileLayer {
    public string url;
    
    public override Task<bool> HasElevationTile (TileId tileId, CancellationToken cancellationToken = new CancellationToken ()) {
        // This method will be called first, before GetElevationTile, to determine if the specified TileId is present.

        // If working with many tiles, it is recommended to create a lookup table in order to quickly determine which
        // tiles are present. If no lookup data structure is available, the ElevationTile data could be requested here
        // instead, and put in a pool that can be consumed when GetElevationTile is called. Regardless of the approach,
        // the map will cache the result of this method, so HasElevationTile is called just once per tile in a layer.
        return Task.FromResult (true);
    }

    public override async Task<ElevationTile> GetElevationTileData (TileId tileId, CancellationToken cancellationToken = new CancellationToken ()) {
        double westLong;
        double eastLong;
        double northLat;
        double southLat;
        tileId.CalculateBounds (out westLong, out eastLong, out southLat, out northLat);
        
        // Request and decode elevation data asynchronously...
        var requestUrl = $"{url}&BBOX={southLat},{westLong},{northLat},{eastLong}";
        var elevationTileData = Array.Empty<byte> ();
        // Run on main thread
        await UnityTaskFactory.StartNew (async () => {
            UnityWebRequest webRequest = UnityWebRequest.Get(requestUrl);
            await webRequest.SendWebRequest ();
            elevationTileData = webRequest.downloadHandler.data;
        }, cancellationToken);
        
        return ElevationTile.FromDataInMeters(tileId, 256, 256, elevationTileData);
    }
}
