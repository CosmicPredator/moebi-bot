namespace Moebi.Bot.Anilist;

public static class Queries
{
    public const string MediaDetailQuery = @"
    query ($id: Int) {
      Media (id: $id) {
        id
        title {
          romaji
          english
          native
        }
        format
        description
        status
        season
        seasonYear
        coverImage {
          color
        }
      }
    }";

    public const string MediaSearchQuery = @"
    query ($search: String, $pageNum: Int, $mediaType: MediaType) {
      Page (perPage: 25, page: $pageNum) {
        pageInfo {
          hasNextPage
          currentPage
        }
        media (search: $search, type: $mediaType) {
          ...MediaDetails
        }
      }
    }

    fragment MediaDetails on Media {
      id
        title {
          romaji
          english
          native
        }
        format
        
    }";

    public const string CharacterSearchQuery = @"
    query ($search: String, $pageNum: Int) {
      Page(perPage: 25, page: $pageNum) {
        pageInfo {
          hasNextPage
          currentPage
        }
        characters(search: $search) {
          name {
            full
            alternative
          }
          id
        }
      }
    }";
}