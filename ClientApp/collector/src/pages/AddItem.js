import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "../api/axios";

const AddItem = () => {
    const navigate = useNavigate();
    const params = useParams();

    const collectionId = params.collectionId.toString();

    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [value, setValue] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const accessToken = localStorage.getItem('accessToken');

            const response = await axios.post("/Item", 
                JSON.stringify({name, description, value, collectionId}),
                {
                    headers: {
                        "Content-Type": "application/json",
                        'Authorization': `Bearer ${accessToken}`
                    }
                }
            );
        } catch (error) {
            console.log(error);
        };

        navigate(`/collections/${collectionId}`);
    };

    return (
        <>
            <form onSubmit={handleSubmit}>
                <label htmlFor="name">Name</label>
                <input type="text" onChange={(e) => setName(e.target.value)} value={name} required/><br />
                <label htmlFor="description">Description</label>
                <input type="text" onChange={(e) => setDescription(e.target.value)} value={description} required/><br />
                <label htmlFor="value">Value</label>
                <input type="text" onChange={(e) => setValue(e.target.value)} value={value} required/><br />
                <button>Add Item</button>
            </form>
        </>
    )
}
export default AddItem;