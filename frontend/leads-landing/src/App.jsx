import { useEffect, useState } from 'react';
import { apiGet, apiNoBody, apiJson } from './api/client';
import InvitedCard from './components/InvitedCard';
import AcceptedCard from './components/AcceptedCard';
import Tabs from './components/Tabs';
import './styles.css';

export default function App() {
  const [tab, setTab] = useState('invited'); 
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [err, setErr] = useState(null);

  const samples = [
    {
      contactFirstName: 'Luis',
      contactLastName: 'Cordeiro',
      contactEmail: 'l11@exemplo.com',
      contactPhone: '(31) 91100-0000',
      category: 'Lavagem De Carro',
      suburb: 'Lourdes',
      description: 'Lavagem ecológica com economia de água',
      price: 1000
    },
    {
      contactFirstName: 'Igor',
      contactLastName: 'Li',
      contactEmail: 'I11@exemplo.com',
      contactPhone: '(31) 91735-3344',
      category: 'Compra de livro',
      suburb: 'Lagos da pampulha',
      description: 'Compra de livro em uma livraria local',
      price: 111
    },
    {
      contactFirstName: 'Leo',
      contactLastName: 'Rocha',
      contactEmail: 'L11@exemplo.com',
      contactPhone: '(31) 91133-5571',
      category: 'Limpador de neve',
      suburb: 'Independência',
      description: 'Ideal para descobrir entradas de garagem coberta de neve',
      price: 700
    }
  ];

  const [createdCount, setCreatedCount] = useState(0);

  async function load(currentTab = tab) {
    try {
      setLoading(true);
      setErr(null);
      const status = currentTab === 'invited' ? 0 : 1; 
      const data = await apiGet(`/api/leads?status=${status}`);
      setItems(data);
    } catch (e) {
      setErr(e.message);
    } finally {
      setLoading(false);
    }
  }

  async function accept(id) {
    await apiNoBody(`/api/leads/${id}/accept`, 'PUT');
    load('invited');  // permanece na guia Invited após aceitar
  }

  async function decline(id) {
    await apiNoBody(`/api/leads/${id}/decline`, 'PUT');
    load('invited');
  }

  async function createSample() {
    const next = samples[createdCount];
    await apiJson('/api/leads', 'POST', next);
    setCreatedCount((prev) => (prev + 1) % samples.length);
    load('invited');
  }

  useEffect(() => { load(); }, [tab]);

  return (
    <div className="container">
      {}
      <link
        rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css"
      />

      <header className="row space">
        <div>
          <h1>Leads</h1>
        </div>
        <div className="row gap">
          {tab === 'invited' && (
            <button onClick={createSample}>
              <i className="fa-solid fa-plus"></i>{' '}
              Criar Lead 
            </button>
          )}
          <button onClick={() => load(tab)}>
            <i className="fa-solid fa-rotate"></i> Recarregar
          </button>
        </div>
      </header>

      <Tabs active={tab} onChange={setTab} />

      {err && <p style={{ color: '#b00' }}>Erro: {err}</p>}
      {loading ? (
        <p className="muted">Carregando...</p>
      ) : items.length === 0 ? (
        <p className="muted">Nenhum lead nesta guia.</p>
      ) : tab === 'invited' ? (
        items.map((l) => (
          <InvitedCard key={l.id} lead={l} onAccept={accept} onDecline={decline} />
        ))
      ) : (
        items.map((l) => <AcceptedCard key={l.id} lead={l} />)
      )}
    </div>
  );
}
