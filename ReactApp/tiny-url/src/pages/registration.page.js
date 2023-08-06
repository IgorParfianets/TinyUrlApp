import {useForm} from "react-hook-form";
import AuthService from "../services/auth.service";
import {useNavigate} from "react-router-dom";
import TokenDto from "../models/dto/token.dto";
import useToken from "../utils/hooks/useToken";

const authService = new AuthService()

export default function Registration() {
    const navigate = useNavigate()
    const {setToken} = useToken()

    const {
        register,
        handleSubmit,
        reset,
        formState:
            {isValid, errors}
    }
        = useForm({
        defaultValues: {
            username: '',
            email: '',
            password: '',
            passwordConfirmation: ''
        }, criteriaMode: "all"
    })

    const handlerSubmitForm = async (data) => {
        if (isValid) {
            const token = await authService.registration(data)
            if (token instanceof TokenDto) {
                setToken(token)
                navigate('/')
            }
        }
        reset()
    }

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-md w-96">
                <h2 className="text-2xl font-bold mb-4">Registration Form</h2>
                <form onSubmit={handleSubmit(handlerSubmitForm)}>
                    <div className="mb-4">
                        <label htmlFor="username" className="block text-gray-800 font-medium">Username</label>
                        <input
                            {...register('username', {required: "Required username"})}
                            id="username"
                            className="w-full border rounded-lg py-2 px-3 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
                            type="text"
                            placeholder="Enter username"/>
                        {errors?.username && <p className="text-red-700 font-thin">{errors.username.message}</p>}
                    </div>

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

                    <div className="mb-4">
                        <label htmlFor="confirmationPassword" className="block text-gray-800 font-medium">
                            Password Confirmation
                        </label>

                        <input
                            {...register('passwordConfirmation',
                                {
                                    required: "Required confirm password",
                                    minLength: {
                                        value: 5, message: 'Min 5 symbols'
                                    }
                                })}
                            id="confirmationPassword"
                            type="password"
                            className="w-full border rounded-lg py-2 px-3 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
                            placeholder="Enter confirm password"
                            autoComplete="new-password"/>
                        {errors?.passwordConfirmation &&
                            <p className="text-red-700 font-thin">{errors.passwordConfirmation.message}</p>}
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
