import {useForm} from "react-hook-form";
import AuthService from "../services/auth.service";
import {useNavigate} from "react-router-dom";
import useToken from "../utils/hooks/useToken";
import TokenDto from "../models/dto/token.dto";

const authService = new AuthService()

export default function Login(){
    const navigate = useNavigate()
    const {token, setToken} = useToken()

    const {
        register,
        handleSubmit,
        reset,
        formState: {
            isValid,
            errors,
        }} = useForm(
            {defaultValues:
                    {email: '', password: ''},
                criteriaMode: 'all'})

    const clickHandler = async (data) => {
        if(isValid){
            const token = await authService.login(data)

            if(token instanceof TokenDto){
                setToken(token)
                navigate(-1)
            }
        }
        reset()
    }

    return (
        <div className="container mx-auto max-w-[500px]">
            <h2>Login</h2>
            <div className="flex">
                <form className="flex flex-col mt-3 py-4 px-2 gap-7 bg-sky-200" onSubmit={handleSubmit(clickHandler)}>

                    <input
                        {...register('email', {required:"Required email"})}
                        type="text"
                        placeholder="Enter email"/>
                    {errors?.email && <p className="text-fuchsia-700">{errors.email.message}</p>}

                    <input
                        {...register('password',
                            {
                                required:"Required password",
                                minLength: {value: 5, message: 'Min 5 symbols'
                                }})}
                        type="password"
                        placeholder="Enter Password"/>
                    {errors?.password && <p className="text-fuchsia-700">{errors.password.message}</p>}
                    <button type="submit">Submit</button>
                </form>
            </div>
        </div>
    )
}