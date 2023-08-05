import { useRef, useState, useEffect, useContext } from "react";
import { Link, useNavigate, useLocation } from 'react-router-dom';

import axios from '../api/axios';
const LOGIN_URL = '/Authentication/Login';

const Login = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const userRef = useRef();
    const errRef = useRef();

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [errorMessage, setErrorMessage] = useState("");

    useEffect(() => {
        userRef.current.focus();
    }, []);

    useEffect(() => {
        setErrorMessage("");
    }, [username, password]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(LOGIN_URL, 
                JSON.stringify({username, password}),
                {
                    headers: { 'Content-Type': 'application/json'},
                    withCredentials: true
                }
            );
            console.log(JSON.stringify(response?.data));

            const refreshToken = response?.data?.refreshToken;
            const token = response?.data?.token;

            localStorage.clear();
            localStorage.setItem('accessToken', response?.data?.token);
            localStorage.setItem('refreshToken', response?.data?.refreshToken);

            const response2 = await axios.get('/Authentication/GetUser', 
                {
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`
                    },
                }
            );
            console.log(JSON.stringify(response2?.data));

            localStorage.setItem('username', response2.data);
            localStorage.setItem('loggedIn', true);

            setUsername('');
            setPassword('');

            navigate('/');
        } catch (error) {
            if (!error?.response) {
                setErrorMessage('No Server Response');
            } else if (error.response?.status === 400) {
                setErrorMessage('Missing Username or Password');
            } else if (error.response?.status === 401) {
                setErrorMessage('Unauthorized');
            } else {
                setErrorMessage('Login Failed');
            }
            errRef.current.focus();
        };
    };

    return (
        <div>
            <p ref={errRef} className={errorMessage ? "errormessage" : "hide"} aria-live="assertive">
                {errorMessage}
            </p>
            <h1>Sign In</h1>
            <form onSubmit={handleSubmit}>
                <label htmlFor="username">Username:</label>
                <input type="text" ref={userRef} autoComplete="off" onChange={(e) => setUsername(e.target.value)} value={username} required />
                <label htmlFor="password">Password:</label>
                <input type="password" onChange={(e) => setPassword(e.target.value)} value={password} required />
                <button>Sign In</button>
            </form>
            <p>
                Need an Account?<br />
                <span className="line">
                    <Link to='register'>Register</Link>
                </span>
            </p>
        </div>
    );
};

export default Login;