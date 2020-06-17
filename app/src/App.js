import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import './index.css';

function App() {
  return (
    <Layout>
      <Route exact path='/' component={Home} />
    </Layout>
  );
}

export default App;

