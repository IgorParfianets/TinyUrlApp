import CustomError from "./custom.error";

export default class UnauthorizedError extends CustomError{
    constructor(message) {
        super("The API response contains status code 401 Unauthorized. " + message);
    }
}