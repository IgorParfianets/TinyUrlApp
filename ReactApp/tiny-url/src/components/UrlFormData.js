
export function UrlFormData({originalUrl, shortUrl, setFormData }){

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