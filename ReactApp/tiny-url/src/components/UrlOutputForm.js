import useToken from "../utils/hooks/useToken";

export function UrlOutputForm({originalUrl, shortUrl, setFormData}) {
    const {token, setToken} = useToken()

    const handleCopyClick = async () => {
        try {
            await navigator.clipboard.writeText(shortUrl);
            alert("Successfully");
        } catch (error) {
            alert("Failed");
        }
    };

    return (
        <div className="container">
            <label>Original URL:</label>
            <input value={originalUrl} disabled/>
            <br/>
            <label>Short URL:</label>
            <input value={shortUrl} disabled/>
            <br/>
            <button className="bg-sky-200 rounded" onClick={() => setFormData(null)}>Another one</button>
            <button className="bg-sky-200 rounded" onClick={handleCopyClick}>Copy</button>
        </div>
    )
}