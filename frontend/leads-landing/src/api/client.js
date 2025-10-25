const API_URL = import.meta.env.VITE_API_URL;

export async function apiGet(path) {
  const res = await fetch(`${API_URL}${path}`);
  if (!res.ok) throw new Error(`GET ${path} => ${res.status}`);
  return res.json();
}

export async function apiNoBody(path, method='PUT') {
  const res = await fetch(`${API_URL}${path}`, { method });
  if (!res.ok && res.status !== 204) {
    const t = await res.text();
    throw new Error(`${method} ${path} => ${res.status} ${t}`);
  }
}

export async function apiJson(path, method='POST', body) {
  const res = await fetch(`${API_URL}${path}`, {
    method,
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body ?? {})
  });
  if (!res.ok && res.status !== 204) {
    const t = await res.text();
    throw new Error(`${method} ${path} => ${res.status} ${t}`);
  }
  return res.status === 204 ? null : res.json();
}
