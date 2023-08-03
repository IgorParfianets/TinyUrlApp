import {useForm} from "react-hook-form";
import AuthService from "../services/auth.service";

const authService = new AuthService()

export default function Login(){
    const {
        register,
        handleSubmit,
        reset,
        formState: {
            isValid,
            errors,
        }} = useForm({defaultValues: {email: '', password: ''}, criteriaMode: 'all'})

    const clickHandler = async (data) => {
        if(isValid){
            try{
                await authService.login(data)
                reset()
            }catch (e){

            }
        }
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