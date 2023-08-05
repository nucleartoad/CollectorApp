import axios from "../api/axios";
import { Link } from "react-router-dom";
import { useEffect, useState } from "react";

const Collections = () => {
    const [collections, setCollections] = useState([]);

    useEffect(() => {
        const accessToken = localStorage.getItem('accessToken');

        (async () => {
            const response = await axios.get('/Collection',{
                headers: {
                    'Authorization': `Bearer ${accessToken}`
                }
            });
            setCollections(response.data);
        })();
    }, [])

    return (
        <>
            <p>this is the collections page</p>
            <Link to='create-collection'>Create New Collection</Link><br /><br />

            {collections.map(collection => {
                return (
                    <div key={collection.id}>
                        <Link to={`/collections/${collection.id}`}>{collection.name}</Link><br />
                    </div>
                )
            })}
        </>
    );
};
export default Collections;