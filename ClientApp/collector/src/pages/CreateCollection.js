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
    }

    return (
        <div>
            <h1>this is the create collection page</h1>
            <form onSubmit={handleSubmit}>
                <label htmlFor="name">Name</label>
                <input type="text" onChange={(e) => setName(e.target.value)} value={name} required/>
                <label htmlFor="description">Description</label>
                <input type="text" onChange={(e) => setDescription(e.target.value)} value={description} required/>
                <button>Create</button>
            </form>
        </div>
    );
};
export default CreateCollection;