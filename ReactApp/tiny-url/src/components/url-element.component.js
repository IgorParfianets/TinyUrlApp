export function UrlElement(props) {
    const {
        originalUrl,
        shortUrl,
        alias,
        urlCreated,
        handleRemoveUrl,
    } = props

    return (
        <li className="bg-white shadow-md rounded-lg p-4 m-2 border border-gray-300">
            <p className="text-gray-800 font-medium">Long URL: {originalUrl}</p>
            <p className="text-gray-800 font-medium">Short URL: {shortUrl}</p>
            <p className="text-gray-800 font-medium">Alias: {alias}</p>
            <p className="text-gray-800 font-medium">Created: {urlCreated}</p>
            <button className="bg-amber-500 text-white font-bold py-2 px-4 mt-4 rounded hover:bg-amber-600 focus:outline-none focus:ring-2 focus:ring-amber-500">
                Remove
            </button>
        </li>
        // <li className="bg-sky-200 rounded p-2 m-1">
        //     <p>Long URL: {originalUrl}</p>
        //     <p>Short URL: {shortUrl}</p>
        //     <p>Alias: {alias}</p>
        //     <p>Created: {urlCreated}</p>
        //     <button className="bg-amber-300 rounded" onClick={async () => handleRemoveUrl(alias)}>Remove</button>
        // </li>
    )
}