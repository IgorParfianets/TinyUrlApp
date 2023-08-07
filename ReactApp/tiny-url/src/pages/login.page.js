import {useForm} from "react-hook-form";
import {useNavigate} from "react-router-dom";
import AuthService from "../services/auth.service";
import useToken from "../utils/hooks/useToken";
import TokenDto from "../models/dto/token.dto";
import BadRequestError from "../models/errors/badRequest.error";

const authService = new AuthService()

export default function Login() {
    const navigate = useNavigate()
    const {setToken} = useToken()

    const {
        register,
        handleSubmit,
        reset,
        formState: {
            isValid,
            errors,
        }
    } = useForm(
        {
            defaultValues:
                {email: '', password: ''},
            criteriaMode: 'all'
        })

    const handlerSubmitForm = async (data) => {
        if (isValid) {
            try{
                const token = await authService.login(data)
                if (token instanceof TokenDto) {
                    setToken(token);
                    navigate('/');
                }
            }catch (error){
                if (error instanceof BadRequestError) {
                    console.warn(error.message)
                }
            }
        }
        reset()
    }

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-md w-96">
                <h2 className="text-2xl font-bold mb-4">Login Form</h2>
                <form onSubmit={handleSubmit(handlerSubmitForm)}>
                    <div className="mb-4">
                        <label htmlFor="email" className="block text-gray-800 font-medium">Email</label>
                        <input
                            {...register('email', {required: "Required email"})}
                            id="email"
                            type="text"
                            className="w-full border rounded-lg py-2 px-3 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
                            placeholder="Enter email"/>
                        {errors?.email && <p className="text-red-700 font-thin">{errors.email.message}</p>}
                    </div>

                    <div className="mb-4">
                        <label htmlFor="password" className="block text-gray-800 font-medium">Password</label>
                        <input
                            {...register('password',
                                {
                                    required: "Required password",
                                    minLength: {
                                        value: 5, message: 'Min 5 symbols'
                                    }
                                })}
                            id="password"
                            type="password"
                            className="w-full border rounded-lg py-2 px-3 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
                            placeholder="Enter Password"
                            autoComplete="new-password"/>
                        {errors?.password && <p className="text-red-700 font-thin">{errors.password.message}</p>}
                    </div>

                    <div className="mb-6">
                        <button type="submit"
                                className="w-full bg-blue-500 text-white font-bold py-2 px-4 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 hover:bg-blue-600">
                            Submit
                        </button>
                    </div>
                </form>
            </div>
        </div>
    )
}