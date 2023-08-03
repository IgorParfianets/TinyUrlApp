import CustomError from "./custom.error";

export default class BadRequestError extends CustomError{
    constructor(message) {
        super("The API response contains status code 400. " + message);
    }
}