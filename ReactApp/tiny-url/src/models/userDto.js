import { NIL  } from "uuid";

export default class UserDto {
    id = NIL;
    email = "";
    password = "";

    constructor(id, email, password) {
        this.id = id;
        this.email = email;
        this.password = password;
    }

    static fromResponse(response) {
        return new UserDto(response.id, response.email, null);
    }
}
