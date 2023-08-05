import { useNavigate, useParams } from "react-router-dom";
import axios from "../api/axios";
import { useState } from "react";

const Item = () => {
    const navigate = useNavigate();

    const params = useParams();
    const collectionId = params.collectionId;
    const itemId = params.itemId;
    const accessToken = localStorage.getItem('accessToken');

    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [value, setValue] = useState('');

    try {
        (async () => {
            const response = await axios.get(`/Item/${collectionId}/${itemId}`, {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`
                }
            })
            const data = response.data;

            setName(data.name);
            setDescription(data.description);
            setValue(data.value);
        })();
    } catch (error) {
        console.log(error);
    };

    const removeItem = () => {
        try {
            const response = axios.delete(`/Item/${collectionId}/${itemId}`, {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${accessToken}`
                }
            });

            navigate(`/Collections/${collectionId}`);
        } catch (error) {
            console.log(error);
        };
    };

    return (
        <>
            <h1>{name}</h1>
            <p>{description}</p>
            <p>{value}</p>
            <p>this is the page for item {params.itemId}</p>
            <p>this item is part of the collection {params.collectionId}</p>
            <br />

            <button onClick={removeItem}>Delete Item</button>
        </>
    )
}
export default Item;