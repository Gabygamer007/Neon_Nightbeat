import './css/app.css';
import { BrowserRouter as Router, Routes, Route} from "react-router-dom";
import { Home } from "./pages/home";
import { Register } from "./pages/register";
import { Profile } from "./pages/profile";
import { Leaderboard } from "./pages/leaderboard";
import { Login } from './pages/login';

function App() {
  return (
    <div className="App">
      <Router>
        <Routes>
          <Route path='/' element={<Home />} />
          <Route path='/register' element={<Register />} />
          <Route path='/login' element={<Login />} />
          <Route path='/profile' element={<Profile />} />
          <Route path='/leaderboard' element={<Leaderboard />} />
        </Routes>
      </Router>
    </div>
  );
}

export default App;
