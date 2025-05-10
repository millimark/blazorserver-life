using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorServerApp.Shared
{
    /// <summary>
    /// A service class to interface with the ConwayWebAPI 
    /// </summary>
    public class ConwayService
    {
        private readonly HttpClient _httpClient;

        public ConwayService(HttpClient client)
        {
            _httpClient = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Gets a lifeboard matrix 
        /// </summary>
        /// <param name="pattern">Name of pattern</param>
        /// <returns>a LifeBoardInt object that contains as its initial pattern the
        /// pattern named by pattern
        /// </returns>
        public async Task<LifeBoardInt> GetPatternAsync(string pattern)
        {
            ArgumentNullException.ThrowIfNull(pattern);

            var response = await _httpClient.GetAsync(new Uri($"conwaylife/{pattern}", UriKind.Relative)).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<LifeBoardInt>(responseStream).ConfigureAwait(false);
            return result ?? throw new InvalidOperationException("Failed to deserialize response");
        }

        /// <summary>
        /// Applies rules to a pattern and returns the next generation
        /// </summary>
        /// <param name="cells">current </param>
        /// <returns>the next generation pattern after applying Life rules</returns>
        public async Task<LifeBoardInt> GetNextGeneration(LifeBoardInt cells)
        {
            ArgumentNullException.ThrowIfNull(cells);

            using var ms = new MemoryStream();
            await JsonSerializer.SerializeAsync(ms, cells).ConfigureAwait(false);
            ms.Position = 0;

            using var sr = new StreamReader(ms);
            var str = sr.ReadToEnd();

            using var body = new StringContent(str, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri("conwaylife", UriKind.Relative), body).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await JsonSerializer.DeserializeAsync<LifeBoardInt>(responseStream).ConfigureAwait(false);
            return result ?? throw new InvalidOperationException("Failed to deserialize response");
        }
    }
}
