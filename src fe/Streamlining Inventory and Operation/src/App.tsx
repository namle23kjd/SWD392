import { RouterProvider } from 'react-router-dom';
import { router } from './route';
import { ToastContainer } from 'react-toastify'; // Import ToastContainer
import 'react-toastify/dist/ReactToastify.css'; // Import style của Toastify
import { QueryClientProvider } from '@tanstack/react-query';
import { queryClient } from './util/queryClient';

function App() {
  return (<>
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
    <ToastContainer />
  </>);
}

export default App;