import './App.css';
import AllRoutes from './Route/AllRoutes';
import { AuthProvider } from './Auth/AuthProvider';

const App = () => {
  return (
    <AuthProvider>
      <AllRoutes />
    </AuthProvider>
  );
};

export default App;
