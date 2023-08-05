import {useState} from "react";
import {UrlOutputForm} from "../components/UrlOutputForm";
import {UrlInputForm} from "../components/UrlInputForm";

export default function Home(){
    const [formData, setFormData] = useState(null)

    if(formData){
        return <UrlOutputForm
            originalUrl={formData.originalUrl}
            shortUrl={formData.shortUrl}
            setFormData={setFormData}
        />
    }

    return <UrlInputForm setFormData={setFormData}/>


}