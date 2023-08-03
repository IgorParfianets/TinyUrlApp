import {useForm} from "react-hook-form";
import UrlService from "../services/url.service";
import {useState} from "react";
import {UrlFormData} from "../components/UrlFormData";

const urlService = new UrlService()

export default function Home(){
    const [formData, setFormData] = useState(null)

    const {
        register,
        handleSubmit,
        reset,
        formState: {
            isValid,
            errors,
        }} = useForm({defaultValues: {originalUrl: '', alias: ''}, criteriaMode: 'all'})

    const clickHandler = async (data) => {
        if(isValid){
            const result = await urlService.createShortUrl(data)

            if(result)
                setFormData(result)
            reset()
        }
    }

    if(formData){
        return <UrlFormData
            originalUrl={formData.originalUrl}
            shortUrl={formData.shortUrl}
            setFormData={setFormData}
        />
    }

    return (
        <div className="container mx-auto max-w-[500px]">
            <form onSubmit={handleSubmit(clickHandler)}>
                <label>Original URL: </label>
                <input {...register('originalUrl', {required: 'Required link'})}
                       type='text'
                       placeholder='Enter original link'/>
                {errors?.originalUrl && <p className="text-fuchsia-700">{errors.originalUrl.message}</p>}

                <label>Alias: </label>
                <input{...register('alias', {required: 'Required alias'})}
                      type='text'
                      placeholder='Enter alias'/>
                {errors?.alias && <p className="text-fuchsia-700">{errors.alias.message}</p>}

                <button className="bg-amber-600 rounded" type='submit'>Submit</button>
            </form>
        </div>
    )
}