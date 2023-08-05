import { useState } from "react";
import axios from "../api/axios";
import { useNavigate } from "react-router-dom";

const CreateCollection = () => {
    const navigate = useNavigate();

    const [name, setName] = useState('');
    const [description, setDescription] = useState('');

    const accessToken = localStorage.getItem('accessToken');
    
    const handleSubmit = () => {
        try {
            const response = axios.post('/Collection', 
                JSON.stringify({name, description}),
                {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${accessToken}`
                    }
                }
            );
            console.log(JSON.stringify(response));

            navigate('/collections/created-collection')
        } catch (error) {
            console.log(error);
        };
    };

    const cancel = () => {
        navigate('/collections');
    };

    return (
        <div>
            <h1>Create a new collection</h1>
            <form onSubmit={handleSubmit}>
                <label htmlFor="name">Collection name</label>
                <input type="text" onChange={(e) => setName(e.target.value)} value={name} required/>
                <br />
                <label htmlFor="description">Description</label>
                <input type="text" onChange={(e) => setDescription(e.target.value)} value={description} required/>
                <br />
                <button>Create new Collection</button>
            </form>
            <button onClick={cancel}>Cancel</button>
        </div>
    );
};
export default CreateCollection;