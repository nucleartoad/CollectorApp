import { useNavigate, useParams } from "react-router-dom";
import axios from "../api/axios";
import { useState, useEffect } from "react";
import { Link } from "react-router-dom";

const Collection = () => {
    const navigate = useNavigate();
    const params = useParams();
    const [loaded, setLoaded] = useState(false);

    const [name, setName] = useState('');
    const [description, setDescription] = useState('');

    const [items, setItems] = useState([]);

    const collectionId = params.collectionId;
    const accessToken = localStorage.getItem('accessToken');

    try {
        (async () => {    
            const response = await axios.get(`Collection/${collectionId}`, {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`
                }
            });
            const data = response.data;

            setName(data.name);
            setDescription(data.description);
        })();
    } catch (error) {
        console.log(error);
    };

    useEffect(() => {
        try {
            (async () => {
                const response = await axios.get(`/Item/${collectionId}`, {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${accessToken}`
                    }
                });
                setItems(response.data);
                setLoaded(true);
            })();
        } catch (error) {
            console.log(error);
        };
    }, []);

    const deleteCollection = async () => {
        try {
            const response = await axios.delete(`/Collection/${collectionId}`, {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`
                }
            });

            navigate('/collections');
        } catch (error) {
            console.log(error);
        };
    };

    return (
        <div>
            <h1>{name}</h1>
            <p>{description}</p>
            <h3>Items:</h3>
            {loaded
                ? items.map(item => {
                    return (
                        <div key={item.id}>
                            <Link to={`/collections/${collectionId}/${item.id}`}>{item.name}</Link>
                        </div>
                    );
                })
                : <div>loading items...</div>
            }
            <br />

            <button onClick={deleteCollection}>Delete Collection</button>

            <br />
            <Link to={`/collections/${collectionId}/add-item`}>Add new Item</Link><br />
            <Link to="/collections">Back to all collections</Link><br />

            <br /><Link to="/">Home</Link><br />
            <br />
        </div>
    );
}

export default Collection;