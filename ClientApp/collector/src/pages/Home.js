import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import axios from "../api/axios";

function Home() {
    const loggedIn = localStorage.getItem('loggedIn');

    const navigate = useNavigate();
    const logout = async () => {
        const token = localStorage.getItem('accessToken');
        const refreshToken = localStorage.getItem('refreshToken');
        if(localStorage.getItem('loggedIn') == true) {
            const response = await axios.post('/Authentication/Logout', 
                {
                    Token: token, 
                    RefreshToken: refreshToken,
                    headers: {
                        'Content-Type' : 'application/json',
                    },
                    withCredentials: true
                }
            );
        };
        
        localStorage.clear();
        navigate('/login');
    };

    return (
        <div>
            <h1>This is the User home page</h1>
            <br />
            <Link to="/collections">Collections</Link><br />
            <Link onClick={logout} to="/logout">Logout</Link><br />

            <br />

            <div>{localStorage.getItem('refreshToken')}</div>
            <div>{localStorage.getItem('accessToken')}</div>
            <div>{localStorage.getItem('username')}</div>
        </div>
    );
};

export default Home;