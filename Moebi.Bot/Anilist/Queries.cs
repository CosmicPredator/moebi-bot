namespace Moebi.Bot.Anilist;

public static class Queries
{
    /// <summary>
    /// GraphQL query to retrieve details of a specific media item by ID.
    /// </summary>
    public const string MediaDetailQuery = @"
    query ($id: Int) {
      Media (id: $id) {
        id
        idMal
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
        favourites
        popularity
        favourites
        episodes
        duration
        chapters
        volumes
        averageScore
        siteUrl
        type
        coverImage {
          color
        }
      }
    }";

    /// <summary>
    /// GraphQL query to search for media (anime or manga) based on a search term.
    /// </summary>
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

    /// <summary>
    /// GraphQL query to search for characters based on a search term.
    /// </summary>
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

    public const string CharacterDetailQuery = @"
    query ($id: Int) {
      Character (id: $id) {
        ...CharacterInfo
      }
    }

    fragment CharacterInfo on Character {
      age
      name {
        userPreferred
        alternative
        alternativeSpoiler
        first
        last
        full
      }
      bloodType
      dateOfBirth {
        day
        month
        year
      }
      description(asHtml: true)
      favourites
      gender
      id
      image {
        large
      }
    }";
}