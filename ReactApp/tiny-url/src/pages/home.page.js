import {useState} from "react";
import {UrlOutputForm} from "../components/url-output-form.component";
import {UrlInputForm} from "../components/url-input-form.component";

export default function Home() {
    const [formData, setFormData] = useState(null)

    if (formData) {
        return <UrlOutputForm
            originalUrl={formData.originalUrl}
            shortUrl={formData.shortUrl}
            setFormData={setFormData}
        />
    }

    return <UrlInputForm setFormData={setFormData}/>
}