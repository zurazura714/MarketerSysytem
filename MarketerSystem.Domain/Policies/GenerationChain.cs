namespace MarketerSystem.Domain.Policies;

/// <summary>
/// Owns the MLM upline chain format stored in <c>Distributor.GenerationLinker</c>:
/// comma-separated distributor IDs, root-first / immediate-recommender-last.
/// All encoding and decoding of the chain goes through this class.
/// </summary>
public static class GenerationChain
{
    public const int MaxDepth = 5;
    private const char Separator = ',';

    /// <summary>Decodes a chain into distributor IDs, root-first. Invalid entries are skipped.</summary>
    public static IReadOnlyList<int> Parse(string? chain)
    {
        if (string.IsNullOrWhiteSpace(chain))
        {
            return [];
        }

        var ids = new List<int>();
        foreach (var element in chain.Split(Separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (int.TryParse(element, out int id))
            {
                ids.Add(id);
            }
        }

        return ids;
    }

    /// <summary>Chain for a new distributor recommended by a parent with <paramref name="parentChain"/>.</summary>
    public static string Append(string? parentChain, int parentId) =>
        string.IsNullOrWhiteSpace(parentChain)
            ? parentId.ToString()
            : $"{parentChain}{Separator}{parentId}";

    public static int Depth(string? chain) => Parse(chain).Count;

    /// <summary>True when a recommender's chain is full — recommending would exceed <see cref="MaxDepth"/>.</summary>
    public static bool IsAtMaxDepth(string? chain) => Depth(chain) >= MaxDepth;
}
