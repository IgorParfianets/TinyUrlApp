import {useForm} from "react-hook-form";
import UrlService from "../services/url.service";
import useToken from "../utils/hooks/useToken";
import BadRequestError from "../models/errors/badRequest.error";
import ConflictError from "../models/errors/conflict.error";

const urlService = new UrlService()

export function UrlInputForm({setFormData}) {
    const {token} = useToken()
    const {
        register,
        handleSubmit,
        reset,
        formState: {
            isValid,
            errors,
        }
    } = useForm({
            defaultValues:
                {
                    originalUrl: '',
                    alias: ''
                },
            criteriaMode: 'all'
        }
    )

    const handlerSubmitForm = async (data) => {
        if (isValid) {
            try{
                const response = await urlService.createShortUrl(data, token.accessToken)
                setFormData(response)
            }catch (error) {
                if (error instanceof BadRequestError) {
                    console.warn(error.message)
                } else if (error instanceof ConflictError) {
                    console.warn(error.message)
                }
            }

        }
    }

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-md w-96">
                <h2 className="text-2xl font-bold mb-4">TinyURL Form</h2>
                <form onSubmit={handleSubmit(handlerSubmitForm)}>
                    <div className="mb-4">
                        <label htmlFor="originalUrl" className="block text-gray-800 font-medium">Long URL</label>
                        <input
                            {...register('originalUrl', {required: "Required long URL"})}
                            id="originalUrl"
                            type="text"
                            className="w-full border rounded-lg py-2 px-3 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
                            placeholder="Enter long URL"/>
                        {errors?.originalUrl && <p className="text-red-700 font-thin">{errors.originalUrl.message}</p>}
                    </div>

                    <div className="mb-4">
                        <label htmlFor="alias" className="block text-gray-800 font-medium">Alias</label>
                        <input
                            {...register('alias',
                                {
                                    required: "Required alias",
                                    minLength: {
                                        value: 5, message: 'Min 5 symbols'
                                    }
                                })}
                            id="alias"
                            type="text"
                            className="w-full border rounded-lg py-2 px-3 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
                            placeholder="Enter alias"/>
                        {errors?.alias && <p className="text-red-700 font-thin">{errors.alias.message}</p>}
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