import {useForm} from "react-hook-form";
import AuthService from "../services/auth.service";
import {useNavigate} from "react-router-dom";

const authService = new AuthService()

export default function Registration(){
    const navigate = useNavigate()

    const {register,
        handleSubmit,
        reset,
        formState:
            {isValid, errors}}
        = useForm({defaultValues:{
            username:'',
            email:'',
            password:'',
            passwordConfirmation:''}, criteriaMode: "all"})

    const clickHandler = (data) => {
        if(isValid){
            const response = authService.registration(data) // todo: need to decide why that data
            if(response)
                navigate('/')

            reset()
        }
    }

    return (
        <div className="container mx-auto max-w-[500px]">
            <h2>Registration</h2>
            <div className="flex">
                <form className="flex flex-col mt-3 py-4 px-2 gap-7 bg-sky-200" onSubmit={handleSubmit(clickHandler)}>
                    <input
                        {...register('username', {required:"Required username"})}
                        type="text"
                        placeholder="Enter username"/>
                    {errors?.username && <p className="text-fuchsia-700">{errors.username.message}</p>}

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

                    <input
                        {...register('passwordConfirmation',
                            {
                                required:"Required confirm password",
                                minLength: {value: 5, message: 'Min 5 symbols'
                                }})}
                        type="password"
                        placeholder="Enter confirm password"/>
                    {errors?.passwordConfirmation && <p className="text-fuchsia-700">{errors.passwordConfirmation.message}</p>}
                    <button type="submit">Submit</button>
                </form>
            </div>
        </div>
    )
}

// <input
//     {...register('username', {required:"Required name"})}
//     type="text"
//     placeholder="Enter name"
// />
// {errors?.username && <p className="text-fuchsia-700">{errors.username.message}</p>}