# Bamboo-card - Developer Coding Test

Using ASP.NET Core, implement a RESTful API to retrieve the details of the best
n stories from the Hacker News API, as determined by their score, where
n isspecified by the caller to the API.
The Hacker News API is documented here:
https://github.com/HackerNews/API
.
The IDs for the stories can be retrieved from this URI:
https://hacker-news.firebaseio.com/v0/beststories.json
.
The details for an individual story ID can be retrieved from this URI:
https://hacker-news.firebaseio.com/v0/item/21233041.json
(in this case for the story with ID 21233041)
The API should return an array of the best n stories as returned by the Hacker News API in
descending order of score

## Features

- Asynchronously fetches and caches the stories from Hacker News API;
- Three-tier architecture, separate class library for integrations and helper classes;
- IDistributedCache for efficient caching across distributed systems;
- Appsettings use for often updated data, like cache time, API Url or format;
- Pre-fetching list of all stories on application start (just for our case with small data range);
- Unit tests for validating correct response count;
- Extension methods for converting one model to other across application layers;
- Swagger for API Documentations.


### Prerequisites
- .NET 6 SDK.

### Installation

1. Clone the repository to your local machine;
2. Ensure that .NET 6 SDK is installed on your system;
3. Select DCP project as startup and click Run (Visual studio);
4. Try to trigger endpoint via Swagger or Postman by URL: (GET) /HackerNews/best-stories/7 - with count parameters (where 7 is count of best stories).

### Configurations
Appsettings file example, You can edit these params without code recompilation:
{
  "HackerNewsOptions": {
    "BaseUrl": "https://hacker-news.firebaseio.com/v0/",
    "BestStoriesEndpoint": "beststories.json",
    "ItemEndpoint": "item/{0}.json",
    "CacheResponseForSeconds": 300
  },
}

### Enhancements and Future Work
- AutoMapper for ability to remove extension methods in future, in case with increasing number of models, dtos and responses;
- Remove caching all stories (current implementation just for presentation, but in case with a large amount of data/stories
and in case with always updated comments or score it will be to long);
- Restriction of count of Best stories value per request, eg 50 best stories;
- Adding hangfire background job for refreshing current cached data;
- Retry logic for fetching data from Hacker News API;
- Maybe request Hacker News API owner to add new endpoint with our requirements to get TOP `n` stories based on ordering by score;
- In case with GET All stories response from our API - we need to Implement pagination, as a default in will be 10 items per page (or configurable by front, but no more than 50);
- Add some metrics to track most requested number of best stories, to optimize our cache logic.