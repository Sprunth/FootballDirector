import { useState } from 'react'
import './App.css'

function App() {
  const [llmResponse, setLlmResponse] = useState(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState(null)

  const testLlm = async () => {
    setLoading(true)
    setError(null)
    setLlmResponse(null)

    try {
      const response = await fetch('/api/llm/test', { method: 'POST' })
      const data = await response.json()

      if (response.ok) {
        setLlmResponse(data.text)
      } else {
        setError(data.error || 'Unknown error')
      }
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <>
      <h1>Football Director</h1>
      <div className="card">
        <h2>LLM Test</h2>
        <button onClick={testLlm} disabled={loading}>
          {loading ? 'Generating...' : 'Generate Football Character'}
        </button>

        {error && (
          <div style={{ color: 'red', marginTop: '1rem' }}>
            Error: {error}
          </div>
        )}

        {llmResponse && (
          <div style={{ marginTop: '1rem', textAlign: 'left', padding: '1rem', background: '#1a1a1a', borderRadius: '8px' }}>
            <strong>Generated Character:</strong>
            <p>{llmResponse}</p>
          </div>
        )}
      </div>
    </>
  )
}

export default App
