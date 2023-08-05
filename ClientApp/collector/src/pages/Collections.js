import axios from "../api/axios";
import { Link } from "react-router-dom";
import { useEffect, useState } from "react";

const Collections = () => {
    const [collections, setCollections] = useState([]);
    const [loaded, setLoaded] = useState(false);

    useEffect(() => {
        const accessToken = localStorage.getItem('accessToken');
        try {
            (async () => {
                const response = await axios.get('/Collection',{
                    headers: {
                        'Authorization': `Bearer ${accessToken}`
                    }
                });
                setCollections(response.data);
                setLoaded(true);
            })();
        } catch (error) {
            console.log(error);
        };

    }, [])

    return (
        <>
            <h1>My Collections</h1>
            <Link to='create-collection'>Create New Collection</Link><br />
            <h3>---</h3>
            <br />
            {loaded
                ? collections.map(collection => {
                    return (
                        <div key={collection.id}>
                            <Link to={`/collections/${collection.id}`}>{collection.name}</Link><br />
                        </div>
                    )
                 })
                : <div>loading collections...</div>
            }
            <br /><Link to="/">Home</Link><br />
        </>
    );
};
export default Collections;