export function UrlElement(props) {
    const {
        originalUrl,
        shortUrl,
        alias,
        urlCreated,
        handleRemoveUrl,
    } = props

    const handleCopyClick = async () => {
        try {
            await navigator.clipboard.writeText(shortUrl);
        } catch (error) {
            console.error(error)
        }
    };
    return (
        <li className="bg-white shadow-md rounded-lg p-4 m-2 border border-gray-300">
            <p className="text-gray-800 font-medium">Long URL: {originalUrl}</p>
            <p className="text-gray-800 font-medium">Short URL: {shortUrl}</p>
            <p className="text-gray-800 font-medium">Alias: {alias}</p>
            <p className="text-gray-800 font-medium">Created: {urlCreated.toLocaleString()}</p>
            <button onClick={async () => handleRemoveUrl(alias)}
                    className="bg-amber-500 text-white font-bold py-2 px-4 mt-4 mx-2 rounded hover:bg-amber-600 focus:outline-none focus:ring-2 focus:ring-amber-500">
                Remove URL
            </button>
            <button onClick={handleCopyClick}
                    className="bg-emerald-600 text-white font-bold py-2 px-4 mt-4 mx-2 rounded hover:bg-amber-600 focus:outline-none focus:ring-2 focus:ring-amber-500">
                Copy short URL
            </button>
        </li>
    )
}