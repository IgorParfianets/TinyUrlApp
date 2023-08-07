export function UrlOutputForm({originalUrl, shortUrl, setFormData}) {
    const handleCopyClick = async () => {
        try {
            await navigator.clipboard.writeText(shortUrl);
        } catch (error) {
            console.error(error)
        }
    };

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-md w-96">
                <h2 className="text-2xl font-bold mb-4">Tiny URL</h2>
                    <div className="mb-4">
                        <label htmlFor="originalUrl" className="block text-gray-800 font-medium">Long URL</label>
                        <input
                            id="originalUrl"
                            value={originalUrl}
                            className="w-full border rounded-lg py-2 px-3 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
                            disabled
                        />
                    </div>

                    <div className="mb-4">
                        <label htmlFor="shortUrl" className="block text-gray-800 font-medium">Short URL</label>
                        <input
                            id="shortUrl"
                            value={shortUrl}
                            className="w-full border rounded-lg py-2 px-3 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-500"
                            disabled
                        />
                    </div>

                    <div className="flex justify-evenly mb-6">
                        <button onClick={() => setFormData(null)}
                                className=" bg-blue-500 text-white font-bold py-2 px-4 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 hover:bg-blue-600">
                            Another One
                        </button>
                        <button onClick={handleCopyClick}
                                className="bg-emerald-600 text-white font-bold py-2 px-4 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 hover:bg-amber-300">
                            Copy
                        </button>
                    </div>
            </div>
        </div>
    )
}