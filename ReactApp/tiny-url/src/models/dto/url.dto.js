export default class UrlDto {
    originalUrl = "";
    shortUrl = "";
    alias = "";
    urlCreated = null;

    constructor(originalUrl, shortUrl, alias, urlCreated) {
        this.originalUrl = originalUrl;
        this.shortUrl = shortUrl;
        this.alias = alias;
        this.urlCreated = urlCreated;
    }

    static fromResponse(response) {
        return new UrlDto(
            response.originalUrl,
            response.shortUrl,
            response.alias,
            response.urlCreated);
    }
}