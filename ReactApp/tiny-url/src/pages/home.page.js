import {useState} from "react";
import {UrlOutputForm} from "../components/url-output-form.component";
import {UrlInputForm} from "../components/url-input-form.component";

export default function Home() {
    const [formData, setFormData] = useState(null)

    return (
        formData !== null ? (
            <UrlOutputForm
                originalUrl={formData.originalUrl}
                shortUrl={formData.shortUrl}
                setFormData={setFormData}
            />
        ) : (
            <UrlInputForm setFormData={setFormData} />
        )
    );
}